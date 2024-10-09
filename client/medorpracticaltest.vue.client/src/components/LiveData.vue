<script setup>
import { ref, onMounted } from 'vue';

defineProps({
  msg: {
    type: String,
    required: true,
  },
});

const liveData = ref([]); 
const isLoading = ref(true);

async function fetchData() {
  isLoading.value = true;
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

  
  liveData.value = result
    .map((item) => ({
      time: new Date(item.timestamp).toLocaleString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        hour12: true,
      }),
      date: new Date(item.timestamp).toLocaleDateString('en-US'),
      priceCZK: `CZK ${item.bitcoinPriceCZK.toLocaleString()}`,
      priceEUR: `EUR ${item.bitcoinPriceEUR.toLocaleString()}`,
      priceUSD: `USD ${item.bitcoinPriceUSD.toLocaleString()}`,
      saved: false,
    }))
    .reverse(); 

  isLoading.value = false; 
}

onMounted(() => {
  fetchData();
});
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>🔴 Live Data - {{ liveData[0]?.date || 'Loading...' }}</h3>
    <div class="live-data">
      <div v-if="isLoading" class="skeleton-container">
        <div class="skeleton-row" v-for="n in 15" :key="n">
          <div class="skeleton-info"></div>
          <div class="skeleton-prices"></div>
          <div class="skeleton-button"></div>
        </div>
      </div>
      <div v-else class="data-row" v-for="(data, index) in liveData" :key="index">
        <div class="data-info">
          <span class="data-time">{{ data.time }}</span>
          <span class="data-date">{{ data.date }}</span>
        </div>
        <div class="data-prices">
          <span class="data-price">{{ data.priceCZK }}</span>
          <span class="data-price">{{ data.priceEUR }}</span>
          <span class="data-price">{{ data.priceUSD }}</span>
        </div>
        <button
          class="save-button"
          :class="{ 'saved': data.saved }"
          @click="saveData(index)"
        >
          {{ data.saved ? 'SAVED' : 'SAVE' }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.greetings {
  padding: 20px;
}

.green {
  font-weight: 500;
  font-size: 2.6rem;
  text-align: center;
  margin-bottom: 10px;
}

h3 {
  font-size: 1.2rem;
  text-align: center;
  color: #333;
}
.live-data {
    display: flex;
    flex-direction: column;
    gap: 8px;
    overflow-y: auto;
    max-height: 800px;
  }
  
  .live-data::-webkit-scrollbar {
    width: 10px;
  }
  
  .live-data::-webkit-scrollbar-track {
    background: #ffffff;
    border-radius: 10px;
  }
  
  .live-data::-webkit-scrollbar-thumb {
    background-color: #c0c0c0;
    border-radius: 10px;
    border: 2px solid #ffffff;
    min-height: 100px; 
  }
  
  .skeleton-container {
    display: flex;
    flex-direction: column;
    gap: 8px;
    flex-grow: 1;
  }
  
  .skeleton-row {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 16px;
    background-color: #f0f0f0;
    border-radius: 12px;
    animation: pulse 1.5s infinite;
    height: 70px;
  }
  
  .skeleton-info,
  .skeleton-prices,
  .skeleton-button {
    background-color: #e0e0e0;
    border-radius: 6px;
  }
  
  .skeleton-info {
    width: 20%;
    height: 30px;
  }
  
  .skeleton-prices {
    width: 50%;
    height: 30px;
  }
  
  .skeleton-button {
    width: 15%;
    height: 30px;
  }
  
  @keyframes pulse {
    0% {
      opacity: 1;
    }
    50% {
      opacity: 0.4;
    }
    100% {
      opacity: 1;
    }
  }
  
  .data-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 16px;
      background-color: #ffffff;
      border-radius: 12px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      transition: transform 0.2s ease-in-out, box-shadow 0.2s;
    }
    
    .data-row:nth-child(odd) {
      background-color: #f9f9f9;
    }
    
    .data-row:nth-child(even) {
      background-color: #ffffff;
    }
    
    .data-row:hover {
      transform: translateY(-5px);
      box-shadow: 0 3px 6px rgba(0, 0, 0, 0.075);
    }
  
  .data-info {
    flex: 1;
    display: flex;
    flex-direction: column;
  }
  
  .data-time {
    font-size: 1rem;
    font-weight: bold;
    color: #333;
  }
  
  .data-date {
    font-size: 0.85rem;
    color: #777;
  }
  
  .data-prices {
    flex: 2;
    display: flex;
    flex-direction: column;
    gap: 3px;
  }
  
  .data-price {
    font-size: 0.9rem;
    color: #555;
  }
  
  .save-button {
    background-color: #ffd700;
    border: none;
    border-radius: 8px;
    padding: 10px 15px;
    cursor: pointer;
    font-weight: bold;
    color: #333;
    transition: background-color 0.3s;
  }
  
  .save-button.saved {
    background-color: #32cd32;
    color: white;
  }
  
  @media (min-width: 1024px) {
    .green,
    h3 {
      text-align: left;
    }
  
    .live-data {
      max-width: 600px;
    }
  }
  </style>