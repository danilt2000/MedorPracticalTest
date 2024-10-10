<script setup>
import LiveData from "./components/LiveData.vue";
import Chart from "./components/Chart.vue";
import SavedData from "./components/SavedData.vue";
import BitcoinPrice from "./components/BitcoinPrice.vue";



</script>

<template>
  <main>
    <BitcoinPrice />
    <Chart/>
  <SavedData />
  </main>
  <LiveData />
  
  
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }
  .container {
    min-width: 300px;
    flex-grow: 1;
}
  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
