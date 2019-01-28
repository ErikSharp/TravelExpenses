<template>
  <v-card class="pa-3">
    <h1 class="mb-3">Thank you for choosing to use Manage your Moo-lah</h1>
    <p class="text-xs-center mb-0">You're all down with setup.</p>
    <p class="text-xs-center">Now let's create some transactions...</p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn color="primary" @click="next">LET'S GO</v-btn>
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
