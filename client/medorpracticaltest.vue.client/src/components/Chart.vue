<template>
    <div>
      <canvas ref="myChart"></canvas>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from "vue";
  import Chart from "chart.js/auto";
  import 'chartjs-adapter-date-fns';
  
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
  
    const dataUSD = result.map((item) => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceUSD,
    }));
  
    const dataCZK = result.map((item) => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceCZK,
    }));
  
    const dataEUR = result.map((item) => ({
      x: new Date(item.timestamp).getTime(),
      y: item.bitcoinPriceEUR,
    }));
  
    return { dataUSD, dataCZK, dataEUR };
  }
  
  let chartInstance = null;
  
  const config = {
    type: "line",
    data: {
      datasets: [
        {
          label: "Bitcoin Price (USD)",
          borderColor: "red",
          borderWidth: 1,
          radius: 0,
          data: [],
          fill: true,
          backgroundColor: "rgba(255, 99, 132, 0.2)",
          hidden: true,
        },
        {
          label: "Bitcoin Price (CZK)",
          borderColor: "blue",
          borderWidth: 1,
          radius: 0,
          data: [],
          fill: true,
          backgroundColor: "rgba(0, 0, 255, 0.1)",
          hidden: false,
        },
        {
          label: "Bitcoin Price (EUR)",
          borderColor: "purple",
          borderWidth: 1,
          radius: 0,
          data: [],
          fill: true,
          backgroundColor: "rgba(128, 0, 128, 0.2)",
          hidden: true, 
        },
      ],
    },
    options: {
      interaction: {
        intersect: false,
      },
      plugins: {
        legend: {
          display: true,
          onClick: (e, legendItem, legend) => {
            const index = legendItem.datasetIndex;
            legend.chart.data.datasets.forEach((dataset, i) => {
              dataset.hidden = i !== index; 
            });
            legend.chart.update();
          },
        },
      },
      scales: {
        x: {
          type: "time",
          position: "bottom",
          time: {
            unit: "hour",
          },
          ticks: {
            source: "data",
            autoSkip: true,
            maxRotation: 0,
            callback: function (value) {
              return new Date(value).toLocaleTimeString();
            },
          },
        },
        y: {
          beginAtZero: false,
        },
      },
    },
  };
  
  onMounted(async () => {
    if (myChart.value) {
      const ctx = myChart.value.getContext("2d");
      chartInstance = new Chart(ctx, config);
  
      const { dataUSD, dataCZK, dataEUR } = await fetchData();
  
      chartInstance.data.datasets[0].data = dataUSD;
      chartInstance.data.datasets[1].data = dataCZK;
      chartInstance.data.datasets[2].data = dataEUR;
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
  