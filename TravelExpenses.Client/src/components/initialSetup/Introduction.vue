<template>
  <v-card>
    <h1>Welcome to Manage Your Moo-lah</h1>
    <p>There are a few things we need to do to get you started...</p>
    <v-btn @click="next">LET'S GO</v-btn>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/setWindow', Windows.location)
    }
  }
}
</script>

<style scoped>
</style>
