<template>
  <v-card>
    <h1>Keywords</h1>
    <p>Keywords are a way to futher...</p>
    <v-btn @click="next">NEXT</v-btn>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/setWindow', Windows.keywords2)
    }
  }
}
</script>

<style scoped>
</style>
