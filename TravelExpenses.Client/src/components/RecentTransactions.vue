<template>
  <div>
    <h1 class="white--text">Recent Transactions</h1>
    <v-btn @click="addTransaction">Add</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    addTransaction() {
      this.$emit('addTransaction')
    }
  }
}
</script>

<style scoped>
</style>
