<template>
    <div class="bitcoin-prices-container">
      <h1>Bitcoin</h1>
      <div v-if="bitcoinPriceCZK !== null && bitcoinPriceEUR !== null && bitcoinPriceUSD !== null">
        <p>
          CZK: {{ bitcoinPriceCZK.toLocaleString() }}, 
          EUR: {{ bitcoinPriceEUR.toLocaleString() }}, 
          USD: {{ bitcoinPriceUSD.toLocaleString() }}
        </p>
      </div>
      <div v-else>
        <p>Loading prices...</p>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from "vue";
  
  const bitcoinPriceCZK = ref(null);
  const bitcoinPriceEUR = ref(null);
  const bitcoinPriceUSD = ref(null);
  
  async function fetchCurrentPrice() {
    try {
      const requestOptions = {
        method: "GET",
        redirect: "follow",
      };
  
      const response = await fetch(
        "https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetCurrentPrice",
        requestOptions
      );
  
      if (response.ok) {
        const data = await response.json();
        bitcoinPriceCZK.value = data.bitcoinPriceCZK ?? null;
        bitcoinPriceEUR.value = data.bitcoinPriceEUR ?? null;
        bitcoinPriceUSD.value = data.bitcoinPriceUSD ?? null;
      } else {
        console.error("Failed to fetch the current price");
      }
    } catch (error) {
      console.error("Error fetching the current price:", error);
    }
  }
  
  onMounted(() => {
    fetchCurrentPrice();
  });
  </script>
  
  <style scoped>
  .bitcoin-prices-container {
    text-align: left; 
  }
  
  h1 {
    font-size: 2rem;
  }
  
  p {
    font-size: 1.5rem;
    margin: 5px 0;
  }
  </style>
  