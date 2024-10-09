<template>
    <div>
      <canvas ref="myChart"></canvas>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from "vue";
  import Chart from "chart.js/auto";
  import 'chartjs-adapter-date-fns'; // Импортируем адаптер для временных данных
  
  const myChart = ref(null);
  
  async function fetchData() {
    const now = new Date();
    const oneDayAgo = new Date(now.getTime() - 24 * 60 * 60 * 1000);
    const isoDate = oneDayAgo.toISOString();
  
    const response = await fetch(
      `https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetHistoricalData?startDate=${encodeURIComponent(
        isoDate
      )}`,
      {
        method: "GET",
        redirect: "follow",
      }
    );
  
    const result = await response.json();
  
    const dataCZK = result.map((item) => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceCZK,
    }));
  
    return { dataCZK };
  }
  
  const animation = {
    x: {
      type: "number",
      easing: "linear",
      duration: 1000 / 1000,
      from: NaN,
      delay(ctx) {
        if (ctx.type !== "data" || ctx.xStarted) {
          return 0;
        }
        ctx.xStarted = true;
        return ctx.index * (1000 / 1000);
      },
    },
    y: {
      type: "number",
      easing: "linear",
      duration: 1000 / 1000,
      from: (ctx) => {
        if (ctx.index === 0) {
          return ctx.chart.scales.y.getPixelForValue(0);
        }
        const previousDataPoint = ctx.chart.getDatasetMeta(ctx.datasetIndex).data[
          ctx.index - 1
        ];
        if (!previousDataPoint) {
          return ctx.chart.scales.y.getPixelForValue(0);
        }
        return previousDataPoint.getProps(["y"], true).y;
      },
      delay(ctx) {
        if (ctx.type !== "data" || ctx.yStarted) {
          return 0;
        }
        ctx.yStarted = true;
        return ctx.index * (1000 / 1000);
      },
    },
  };
  
  const config = {
    type: "line",
    data: {
      datasets: [
        {
          label: "Bitcoin Price (CZK)",
          borderColor: "blue",
          borderWidth: 1,
          radius: 0,
          data: [],
          fill: true,
          backgroundColor: "rgba(0, 0, 255, 0.1)",
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
          type: "time", // Устанавливаем тип оси как "time"
          position: "bottom",
          time: {
            unit: "hour", // Настраиваем единицу времени
          },
          ticks: {
            source: "data", // Используем данные для генерации меток
            autoSkip: true,
            maxRotation: 0,
            callback: function (value) {
              return new Date(value).toLocaleTimeString(); // Форматируем метки как время
            },
          },
        },
        y: {
          beginAtZero: false,
          min: 1400000, // Устанавливаем минимальное значение оси Y
        },
      },
    },
  };
  
  onMounted(async () => {
    if (myChart.value) {
      const ctx = myChart.value.getContext("2d");
      const chartInstance = new Chart(ctx, config);
  
      const { dataCZK } = await fetchData();
  
      chartInstance.data.datasets[0].data = dataCZK;
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
  