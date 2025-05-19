<template>
    <div>
      <button @click="showModal = true">ConfigureTransitionRule</button>
  
      <ConfigureTransitionModal
        v-if="showModal"
        :columns="columns"
        :boardId="boardId"
        @close="showModal = false"
      />
    </div>
  </template>
  
  <script>
  import ConfigureTransitionModal from './ConfigureTransitionModal.vue';
  
  export default {
    components: {
      ConfigureTransitionModal
    },
    props: {
      boardId: String,
      columns: Array
    },
    data() {
      return {
        showModal: false
      };
    }
  };
  </script>
  