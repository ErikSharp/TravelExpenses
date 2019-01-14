<template>
  <v-card>
    <h1>Categories</h1>
    <p>Categories are used to...</p>
    <v-btn @click="next">NEXT</v-btn>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/setWindow', Windows.categories2)
    }
  }
}
</script>

<style scoped>
</style>
