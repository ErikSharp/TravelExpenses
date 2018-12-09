<template>
  <div>
    <h1 class="white--text">Recent Transactions</h1>
    <v-btn @click="addTransaction">Add</v-btn>
    <v-btn :loading="busy" :disabled="busy" @click="retrieveValues">Retrieve values</v-btn>
    <p v-show="busy">We are busy</p>
    <pre class="white--text">{{values}}</pre>
  </div>
</template>

<script>
export default {
  methods: {
    addTransaction() {
      this.$emit('addTransaction')
    },
    retrieveValues() {
      this.$store.dispatch('Values/retrieveValues')
    }
  },
  computed: {
    busy() {
      return this.$store.state.Values.busy
    },
    values() {
      return this.$store.state.Values.values
    }
  }
}
</script>

<style scoped>
</style>
