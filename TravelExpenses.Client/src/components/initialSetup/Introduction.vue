<template>
  <v-card class="pa-3">
    <h1 class="text-xs-center">Welcome to <br />Manage Your Moo-lah</h1>
    <p class="text-xs-center">
      There are a few things we need to do to get you started...
    </p>
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

<style scoped></style>
