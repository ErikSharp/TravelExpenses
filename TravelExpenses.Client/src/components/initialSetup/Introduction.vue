<template>
  <v-card class="pa-3">
    <h1 class="text-xs-center">Welcome to
      <br>Manage Your Moo-lah
    </h1>
    <p class="text-xs-center">There are a few things we need to do to get you started...</p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn color="primary" @click="next">LET'S GO</v-btn>
      </v-layout>
    </v-flex>
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
      if (!this.$store.state.Location.locations.length) {
        return Windows.location
      } else if (!this.$store.state.Category.categories.length) {
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
