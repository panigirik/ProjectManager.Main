<template>
    <el-form @submit.prevent="addRule">
      <el-select v-model="fromColumn" placeholder="From Column">
        <el-option v-for="col in columns" :key="col.id" :label="col.name" :value="col.id" />
      </el-select>
      <el-select v-model="toColumn" placeholder="To Column">
        <el-option v-for="col in columns" :key="col.id" :label="col.name" :value="col.id" />
      </el-select>
      <el-button type="primary" native-type="submit">Add Rule</el-button>
    </el-form>
  </template>
  
  <script>
  export default {
    data() {
      return {
        columns: [],
        fromColumn: null,
        toColumn: null
      };
    },
    created() {
      this.fetchColumns();
    },
    methods: {
      async fetchColumns() {
        const response = await this.$axios.get('/api/Columns/column');
        this.columns = response.data;
      },
      addRule() {
        this.$axios.post('/api/TicketTransition/rule', {
          fromColumnId: this.fromColumn,
          toColumnId: this.toColumn
        });
      }
    }
  };
  </script>
  