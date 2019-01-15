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
      this.$store.dispatch('InitialSetup/setWindow', this.getNextWindow())
    },
    getNextWindow() {
      if (!this.$store.state.InitialSetup.baseData.hasLocation) {
        return Windows.location
      } else if (!this.$store.state.InitialSetup.baseData.hasCategory) {
        return Windows.categories1
      } else {
        return Windows.keywords1
      }
    }
  }
}
</script>

<style scoped>
</style>
