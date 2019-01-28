<template>
  <v-card class="pa-3">
    <v-flex xs12 class="mb-3">
      <v-layout row align-center>
        <v-avatar size="55" class="mr-3 elevation-4">
          <v-icon large class="primary white--text">my_location</v-icon>
        </v-avatar>
        <h1>Keywords</h1>
      </v-layout>
    </v-flex>
    <p>Keywords are a way to further describe a transaction. A transaction must be tied to one Category, but can have zero or more Keywords.</p>
    <p>
      <strong>e.g.</strong> Today I took an expensive Uber taxi to the airport.
    </p>
    <p>
      In this example,
      <strong>Transportation</strong> would be the chosen Category, and
      <strong>'User'</strong> and
      <strong>'Expensive'</strong> could be the chosen Keywords.
    </p>
    <p>Keywords also allow you to query associated data.</p>
    <p>
      <strong>e.g.</strong> How much did we spend on coffee in Chiang Mai, Thailand?
    </p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn color="primary" @click="next">NEXT</v-btn>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/nextWindow')
    }
  }
}
</script>

<style scoped>
</style>
