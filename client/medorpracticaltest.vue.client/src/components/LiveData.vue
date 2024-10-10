<script setup>
import { ref, onMounted } from "vue";
import { savedDataStore } from "../store";

defineProps({
  msg: {
    type: String,
    required: true,
  },
});

const liveData = ref([]);
const isLoading = ref(true);

function normalizeTimestamp(timestamp) {
  return new Date(timestamp).toISOString().split(".")[0] + "Z";
}

async function fetchData() {
  isLoading.value = true;
  const now = new Date();
  const oneDayAgo = new Date(now.getTime() - 24 * 60 * 60 * 1000);
  const isoDate = oneDayAgo.toISOString();

  try {
    const historicalResponse = await fetch(
      `https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetHistoricalData?startDate=${encodeURIComponent(
        isoDate
      )}`,
      {
        method: "GET",
        redirect: "follow",
      }
    );

    const historicalResult = await historicalResponse.json();

    const savedResponse = await fetch(
      `https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetSavedData`,
      {
        method: "GET",
        redirect: "follow",
      }
    );

    const savedResult = await savedResponse.json();
    const savedTimestamps = new Set(
      savedResult.map((item) => normalizeTimestamp(item.timestamp))
    );

    liveData.value = historicalResult
      .map((item) => ({
        time: new Date(item.timestamp).toLocaleString("en-US", {
          hour: "2-digit",
          minute: "2-digit",
          hour12: true,
        }),
        date: new Date(item.timestamp).toLocaleDateString("en-US"),
        priceCZK: `CZK ${item.bitcoinPriceCZK.toLocaleString()}`,
        priceEUR: `EUR ${item.bitcoinPriceEUR.toLocaleString()}`,
        priceUSD: `USD ${item.bitcoinPriceUSD.toLocaleString()}`,
        saved: savedTimestamps.has(normalizeTimestamp(item.timestamp)), 
        timestamp: item.timestamp,
      }))
      .reverse();

    isLoading.value = false;
  } catch (error) {
    console.error("Error fetching data:", error);
    isLoading.value = false;
  }
}

async function saveData(index) {
  const dataToSave = liveData.value[index];

  const dateTimeString = `${dataToSave.date} ${dataToSave.time}`;
  const localTimestamp = new Date(dateTimeString);

  if (isNaN(localTimestamp.getTime())) {
    console.error("Invalid date format:", dateTimeString);
    return;
  }

  const formattedTimestamp = `${localTimestamp.getFullYear()}-${(
    localTimestamp.getMonth() + 1
  )
    .toString()
    .padStart(2, "0")}-${localTimestamp
    .getDate()
    .toString()
    .padStart(2, "0")}T${localTimestamp
    .getHours()
    .toString()
    .padStart(2, "0")}:${localTimestamp
    .getMinutes()
    .toString()
    .padStart(2, "0")}:${localTimestamp
    .getSeconds()
    .toString()
    .padStart(2, "0")}.000`;

  const payload = {
    bitcoinPriceUSD: parseFloat(dataToSave.priceUSD.replace(/[^\d.-]/g, "")),
    bitcoinPriceEUR: parseFloat(dataToSave.priceEUR.replace(/[^\d.-]/g, "")),
    bitcoinPriceCZK: parseFloat(dataToSave.priceCZK.replace(/[^\d.-]/g, "")),
    timestamp: formattedTimestamp, 
    note: "",
  };

  try {
    const response = await fetch(
      "https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/SaveLiveData",
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      }
    );

    if (response.ok) {
      const idBitcoin = await response.json();
      
      const formattedDate = new Date(dataToSave.timestamp).toLocaleString(
        "en-US",
        {
          year: "numeric",
          month: "2-digit",
          day: "2-digit",
          hour: "2-digit",
          minute: "2-digit",
          second: "2-digit",
          hour12: true,
        }
      );

      liveData.value[index].saved = true;
      savedDataStore.savedData.push({
        id: idBitcoin,
        date: formattedDate,
        priceCZK: dataToSave.priceCZK,
        priceEUR: dataToSave.priceEUR,
        priceUSD: dataToSave.priceUSD,
        note: "",
      });
    } else {
      console.error("Failed to save data");
    }
  } catch (error) {
    console.error("Error saving data:", error);
  }
}

onMounted(() => {
  fetchData();
});
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>🔴 Live Data - {{ liveData[0]?.date || "Loading..." }}</h3>
    <div class="live-data">
      <div v-if="isLoading" class="skeleton-container">
        <div class="skeleton-row" v-for="n in 15" :key="n">
          <div class="skeleton-info"></div>
          <div class="skeleton-prices"></div>
          <div class="skeleton-button"></div>
        </div>
      </div>
      <div
        v-else
        class="data-row"
        v-for="(data, index) in liveData"
        :key="index"
      >
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
          :class="{ saved: data.saved }"
          @click="!data.saved && saveData(index)"
        >
          {{ data.saved ? "SAVED" : "SAVE" }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
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
  width: 80px; 
  text-align: center;
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
    max-width: auto;
  }
}
</style>