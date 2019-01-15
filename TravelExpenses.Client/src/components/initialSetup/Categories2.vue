<template>
  <v-card>
    <h1>Categories</h1>
    <p>The following is a pre-defined list...</p>
    <v-btn @click="next">NEXT</v-btn>
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
      if (!this.$store.state.InitialSetup.baseData.hasKeyword) {
        return Windows.keywords1
      } else {
        return Windows.finish
      }
    }
  }
}
</script>

<style scoped>
</style>
