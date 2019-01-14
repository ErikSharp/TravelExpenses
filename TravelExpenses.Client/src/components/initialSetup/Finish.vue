<template>
  <v-card>
    <h1>Thank you for choosing to use Manage your Moo-lah</h1>
    <p>You're all donw with setup.</p>
    <p>Now let's create some transactions...</p>
    <v-btn @click="next">LET'S GO</v-btn>
  </v-card>
</template>

<script>
import Windows from '@/common/enums/InitialSetupWindows.js'

export default {
  methods: {
    next() {
      this.$store.dispatch('InitialSetup/setWindow', Windows.transactions)
    }
  }
}
</script>

<style scoped>
</style>
