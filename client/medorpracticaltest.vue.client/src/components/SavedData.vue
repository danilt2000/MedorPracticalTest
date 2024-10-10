<script setup>
import { ref, computed, onMounted } from "vue";
import { savedDataStore } from "../store";

const savedData = computed(() => savedDataStore.savedData);
const maxNoteLength = 33;

async function fetchSavedData() {
  try {
    const response = await fetch(
      "https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/GetSavedData",
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      }
    );

    if (response.ok) {
      const result = await response.json();
      savedDataStore.savedData = result.map((item) => ({
        date: new Date(item.timestamp).toLocaleString("en-US", {
          year: "numeric",
          month: "2-digit",
          day: "2-digit",
          hour: "2-digit",
          minute: "2-digit",
          second: "2-digit",
        }),
        id: item.id,
        priceCZK: item.bitcoinPriceCZK,
        priceEUR: item.bitcoinPriceEUR,
        priceUSD: item.bitcoinPriceUSD,
        note: item.note,
        isEditing: false,
      }));
    } else {
      console.error("Failed to fetch saved data");
    }
  } catch (error) {
    console.error("Error fetching saved data:", error);
  }
}

function startEditing(index) {
  savedDataStore.savedData[index].isEditing = true;
}

async function updateNote(id, note) {
  try {
    const response = await fetch(
      "https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/UpdateNote",
      {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ id, note }),
      }
    );

    if (!response.ok) {
      console.error("Failed to update note");
    }
  } catch (error) {
    console.error("Error updating note:", error);
  }
}

function stopEditing(index) {
  const note = savedDataStore.savedData[index].note;
  if (savedDataStore.savedData[index].note.length > maxNoteLength) {
    alert(`Note exceeds the maximum length of ${maxNoteLength} characters.`);
    return;
  }
  savedDataStore.savedData[index].isEditing = false;
  updateNote(savedDataStore.savedData[index].id, note);
}

function checkNoteLength(index) {
  if (savedDataStore.savedData[index].note.length > maxNoteLength) {
    savedDataStore.savedData[index].note = savedDataStore.savedData[
      index
    ].note.substring(0, maxNoteLength);
  }
}

async function deleteEntry(index) {
  const itemId = savedDataStore.savedData[index].id;

  if (confirm("Are you sure you want to delete this entry?")) {
    try {
      const response = await fetch(
        "https://medorbackend.hepatico.ru/api/v1/BitcoinPrice/DeleteSavedData",
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ id: itemId }),
        }
      );

      if (response.ok) {
        savedDataStore.savedData.splice(index, 1);
        console.log("Entry deleted successfully");
      } else {
        console.error("Failed to delete entry");
      }
    } catch (error) {
      console.error("Error deleting entry:", error);
    }
  }
}

onMounted(() => {
  fetchSavedData();
});
</script>

<template>
  <div class="saved-data-container">
    <h2>Saved Data</h2>
    <div class="saved-data-table-wrapper">
      <table>
        <thead>
          <tr>
            <th class="date-column">Notes</th>
            <th class="price-column"></th>
            <th class="price-column"></th>
            <th class="price-column"></th>
            <th class="note-column"></th>
            <th class="actions-column"></th>
          </tr>
        </thead>
      </table>
      <div class="saved-data-scroll">
        <table>
          <tbody>
            <tr v-for="(item, index) in savedData" :key="index">
              <td>{{ item.date }}</td>
              <td>{{ item.priceCZK }} CZK</td>
              <td>{{ item.priceEUR }} EUR</td>
              <td>{{ item.priceUSD }} USD</td>
              <td>
                <div
                  v-if="!item.isEditing"
                  class="editable-note"
                  @click="startEditing(index)"
                >
                  <span class="note-text">{{
                    item.note || "Click to add note"
                  }}</span>
                </div>
                <div v-else>
                  <textarea
                    v-model="item.note"
                    @input="checkNoteLength(index)"
                    @blur="stopEditing(index)"
                    class="edit-input"
                    placeholder="Enter your note here..."
                  />
                </div>
              </td>
              <td>
                <button @click="deleteEntry(index)" class="delete-button">
                  🗑️
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
.saved-data-container {
  margin-top: 1rem;
}

.saved-data-table-wrapper {
  position: relative;
}

.saved-data-scroll {
  max-height: 176px;
  max-width: 100%;
  overflow-y: auto;
  overflow-x: auto;
  box-sizing: border-box;
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
  position: sticky;
  top: 0;
  z-index: 2;
}

th,
td {
  padding: 0.5rem 0.5rem;
  text-align: left;
  border-bottom: 1px solid #e0e0e0;
}

.date-column {
  width: 10%;
  height: 58px;
}

.price-column {
  width: 10%;
}

.note-column {
  width: 50%;
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
