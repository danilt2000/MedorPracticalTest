<script setup>
import { ref } from 'vue';

defineProps({
  msg: {
    type: String,
    required: true,
  },
});

const liveData = ref([
  { time: '19:10', price: 'CZK 1,471,929', saved: false },
  { time: '19:05', price: 'CZK 1,471,929', saved: false },
  { time: '19:00', price: 'CZK 1,471,929', saved: false },
  { time: '18:55', price: 'CZK 1,471,929', saved: false },
  { time: '18:50', price: 'CZK 1,471,929', saved: false },
  { time: '18:45', price: 'CZK 1,471,929', saved: false },
  { time: '18:40', price: 'CZK 1,471,929', saved: false },
  { time: '18:35', price: 'CZK 1,471,929', saved: false },
]);

const saveData = (index) => {
  liveData.value[index].saved = !liveData.value[index].saved;
};
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>🔴 Live Data</h3>
    <div class="live-data">
      <div class="data-row" v-for="(data, index) in liveData" :key="index">
        <span class="data-time">{{ data.time }}</span>
        <span class="data-price">{{ data.price }}</span>
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
  gap: 10px;
  max-height: 400px;
  overflow-y: auto;
}

.data-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 15px;
  background-color: #f2f2f2;
  border-radius: 5px;
}

.data-time,
.data-price {
  font-size: 1rem;
}

.save-button {
  background-color: #ffd700;
  border: none;
  border-radius: 5px;
  padding: 5px 10px;
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
    max-width: 500px;
  }
}
</style>
