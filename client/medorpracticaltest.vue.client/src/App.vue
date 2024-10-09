<script setup>
import HelloWorld from "./components/HelloWorld.vue";
import TheWelcome from "./components/TheWelcome.vue";
import LiveData from "./components/LiveData.vue";
import Chart from "./components/Chart.vue";
import SavedData from "./components/SavedData.vue";
</script>

<template>
  <main>
    <h1>Bitcoin</h1>
    <h1>CZK 1,471,929</h1>
    <Chart/>
  <SavedData />
    <!-- <TheWelcome /> -->
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
