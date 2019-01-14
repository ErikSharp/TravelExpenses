<template>
  <v-card>
    <h1>Country and Location</h1>
    <v-btn @click="next">NEXT</v-btn>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/setWindow', Windows.categories1)
    }
  }
}
</script>

<style scoped>
</style>
