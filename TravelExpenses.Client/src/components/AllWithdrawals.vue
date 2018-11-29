<template>
  <div>
    <h3>All Withdrawals</h3>
    <v-btn @click="addNew">Add New</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    addNew() {
      this.$emit('addNew')
    }
  }
}
</script>

<style scoped>
</style>
