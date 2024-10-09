<template>
    <div class="saved-data">
      <h2>Saved Data</h2>
      <table>
        <thead>
          <tr>
            <th>Date</th>
            <th>Price</th>
            <th>Note</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in data" :key="index">
            <td>{{ item.date }}</td>
            <td>{{ item.price }}</td>
            <td>{{ item.note }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  
  const data = ref([
    { date: '19:10', price: 'CZK 1,471,929', note: 'Price has stabilized after a sharp rise' },
    { date: '19:05', price: 'CZK 1,471,929', note: 'Market started to decline after news of regulatory changes' },
    { date: '19:00', price: 'CZK 1,471,929', note: 'Price increase following a surge in trading volume' },
    { date: '18:55', price: 'CZK 1,471,929', note: 'Bitcoin reached a new monthly high' },
    { date: '18:50', price: 'CZK 1,471,929', note: 'Sharp decline due to mass sell-off' },
    { date: '18:45', price: 'CZK 1,471,929', note: 'Correction after a strong rally' },
    { date: '18:40', price: 'CZK 1,471,929', note: 'Positive impact of Federal Reserve decisions' },
  ]);
  </script>
  
  <style scoped>
  .saved-data {
    margin-top: 1rem;
  }
  
  h2 {
    font-size: 1.5rem;
    margin-bottom: 0.5rem;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
  }
  
  thead {
    background-color: #f4f6f8;
  }
  
  th, td {
    padding: 0.75rem 1rem;
    text-align: left;
    border-bottom: 1px solid #e0e0e0;
  }
  
  tbody tr:nth-child(even) {
    background-color: #f9fafb;
  }
  
  tbody tr:hover {
    background-color: #eef3f7;
  }
  
  td {
    font-size: 0.9rem;
  }
  
  th {
    font-weight: 600;
  }
  </style>
  