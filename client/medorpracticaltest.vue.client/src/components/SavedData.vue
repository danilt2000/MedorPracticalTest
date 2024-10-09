<template>
    <div class="saved-data">
      <h2>Saved Data</h2>
      <table>
        <thead>
          <tr>
            <th class="date-column">Date</th>
            <th class="price-column">Price (CZK)</th>
            <th class="price-column">Price (EUR)</th>
            <th class="price-column">Price (USD)</th>
            <th class="note-column">Note</th>
            <th class="actions-column">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in savedData" :key="index">
            <td>{{ item.date }}</td>
            <td>{{ item.priceCZK }}</td>
            <td>{{ item.priceEUR }}</td>
            <td>{{ item.priceUSD }}</td>
            <td>
              <div v-if="!item.isEditing" class="editable-note" @click="startEditing(index)">
                <span class="note-text">{{ item.note || 'Click to add note' }}</span>
                <span class="tooltip">Click to edit</span>
              </div>
              <div v-else>
                <textarea
                  v-model="item.note"
                  @input="checkNoteLength(index)"
                  @blur="stopEditing(index)"
                  @keyup.enter="stopEditing(index)"
                  class="edit-input"
                  placeholder="Enter your note here..."
                />
                <div v-if="item.note.length > maxNoteLength" class="warning">
                  Note is too long ({{ item.note.length }}/{{ maxNoteLength }} characters). Please shorten it.
                </div>
              </div>
            </td>
            <td>
              <button @click="deleteEntry(index)" class="delete-button">🗑️</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
  
  <script setup>
  import { ref, computed } from 'vue';
  import { savedDataStore } from '../store'; // Import the shared store
  
  const savedData = computed(() => savedDataStore.savedData);
  const maxNoteLength = 45; // Maximum allowed characters for the note
  
  function startEditing(index) {
    savedDataStore.savedData[index].isEditing = true;
  }
  
  function stopEditing(index) {
    // Prevent exiting edit mode if the note is too long
    if (savedDataStore.savedData[index].note.length > maxNoteLength) {
      alert(`Note exceeds the maximum length of ${maxNoteLength} characters.`);
      return;
    }
    savedDataStore.savedData[index].isEditing = false;
  }
  
  function checkNoteLength(index) {
    if (savedDataStore.savedData[index].note.length > maxNoteLength) {
      savedDataStore.savedData[index].note = savedDataStore.savedData[index].note.substring(0, maxNoteLength);
    }
  }
  
  function deleteEntry(index) {
    if (confirm('Are you sure you want to delete this entry?')) {
      savedDataStore.savedData.splice(index, 1);
    }
  }
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
    padding: 0.5rem 0.5rem;
    text-align: left;
    border-bottom: 1px solid #e0e0e0;
  }
  
  .date-column {
    width: 10%;
  }
  
  .price-column {
    width: 10%;
  }
  
  .note-column {
    width: 50%; /* Allocate more space to the note column */
  }
  
  .actions-column {
    width: 5%;
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
  
  .editable-note {
    position: relative;
    cursor: pointer;
    color: #007bff;
    min-height: 1.5rem;
  }
  
  .note-text {
    display: block;
    max-width: 100%;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
  
  .tooltip {
    visibility: hidden;
    background-color: #333;
    color: #fff;
    text-align: center;
    border-radius: 5px;
    padding: 5px;
    position: absolute;
    bottom: 100%;
    left: 50%;
    transform: translateX(-50%);
    margin-bottom: 5px;
    font-size: 0.8rem;
    white-space: nowrap;
    z-index: 1;
  }
  
  .editable-note:hover .tooltip {
    visibility: visible;
  }
  
  .edit-input {
    width: 100%;
    height: 2.5rem;
    resize: none;
    overflow: hidden;
    font-size: 0.9rem;
    border: 1px solid #ccc;
    border-radius: 4px;
    padding: 0.5rem;
    box-sizing: border-box;
  }
  
  .warning {
    color: #dc3545;
    font-size: 0.8rem;
    margin-top: 0.5rem;
  }
  
  .delete-button {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 1.2rem;
  }
  
  .delete-button:hover {
    color: #dc3545;
  }
  </style>
  