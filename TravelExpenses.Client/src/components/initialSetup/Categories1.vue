<template>
  <v-card class="pa-3">
    <v-flex xs12 class="mb-3">
      <v-layout row align-center>
        <v-avatar size="55" class="mr-3 elevation-4">
          <v-icon large class="primary white--text">collections_bookmark</v-icon>
        </v-avatar>
        <h1>Categories</h1>
      </v-layout>
    </v-flex>
    <p>Categories are used to differentiate types of transactions.</p>
    <p>They are used to answer a question, such as:</p>
    <p>'How much did I spend on dining while I was in Hong Kong?'</p>
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
