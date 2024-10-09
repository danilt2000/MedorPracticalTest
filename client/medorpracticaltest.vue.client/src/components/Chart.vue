<template>
    <div>
      <canvas ref="myChart"></canvas>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import Chart from 'chart.js/auto';
  
  const myChart = ref(null);
  
  // Функция для загрузки данных из API
  async function fetchData() {
    // const response = await fetch('https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetHistoricalData?startDate=2024-10-07T00%3A00%3A00Z');
    
    const response = await fetch(
      'https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetHistoricalData?startDate=2024-10-07T00%3A00%3A00Z',
      {
        method: "GET",
        redirect: "follow"
      }
    );

//     const requestOptions = {
//   method: "GET",
//   redirect: "follow"
// };

// fetch("https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetHistoricalData?startDate=2024-10-07T00%3A00%3A00Z", requestOptions)
//   .then((response) => response.text())
//   .then((result) => console.log(result))
//   .catch((error) => console.error(error));
    
    const result = await response.json();
  
    // Преобразуем данные в формат, подходящий для графика
    const data = result.map(item => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceUSD
    }));
  
    const data2 = result.map(item => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceCZK
    }));
  
    return { data, data2 };
  }
  
  const totalDuration = 1000;
  const delayBetweenPoints = totalDuration / 1000;
  const previousY = (ctx) => {
    if (ctx.index === 0) {
      return ctx.chart.scales.y.getPixelForValue(100);
    }
    const previousDataPoint = ctx.chart.getDatasetMeta(ctx.datasetIndex).data[ctx.index - 1];
    if (!previousDataPoint) {
      return ctx.chart.scales.y.getPixelForValue(100);
    }
    return previousDataPoint.getProps(['y'], true).y;
  };
  
  const animation = {
    x: {
      type: 'number',
      easing: 'linear',
      duration: delayBetweenPoints,
      from: NaN,
      delay(ctx) {
        if (ctx.type !== 'data' || ctx.xStarted) {
          return 0;
        }
        ctx.xStarted = true;
        return ctx.index * delayBetweenPoints;
      },
    },
    y: {
      type: 'number',
      easing: 'linear',
      duration: delayBetweenPoints,
      from: previousY,
      delay(ctx) {
        if (ctx.type !== 'data' || ctx.yStarted) {
          return 0;
        }
        ctx.yStarted = true;
        return ctx.index * delayBetweenPoints;
      },
    },
  };
  
  const config = {
    type: 'line',
    data: {
      datasets: [
        {
          label: 'Bitcoin Price (USD)',
          borderColor: 'red',
          borderWidth: 1,
          radius: 0,
          data: [],
        },
        {
          label: 'Bitcoin Price (CZK)',
          borderColor: 'blue',
          borderWidth: 1,
          radius: 0,
          data: [],
        },
      ],
    },
    options: {
      animation,
      interaction: {
        intersect: false,
      },
      plugins: {
        legend: {
          display: true,
        },
      },
      scales: {
        x: {
          type: 'linear',
          position: 'bottom',
          ticks: {
            callback: function(value) {
              return new Date(value).toLocaleTimeString();
            }
          }
        },
        y: {
          beginAtZero: true
        }
      },
    },
  };
  
  onMounted(async () => {
    if (myChart.value) {
      const ctx = myChart.value.getContext('2d');
      const chartInstance = new Chart(ctx, config);
  
      // Загрузка данных из API
      const { data, data2 } = await fetchData();
  
      // Обновляем данные графика
      chartInstance.data.datasets[0].data = data;
      chartInstance.data.datasets[1].data = data2;
      chartInstance.update();
    }
  });
  </script>
  
  <style scoped>
  canvas {
    max-width: 100%;
    height: 400px;
  }
  </style>
  