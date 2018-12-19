<template>
  <div>Locations
    <v-spacer></v-spacer>
    <v-btn @click="cancel">Cancel</v-btn>
  </div>
</template>

<script>
import SetupWindow from '@/common/enums/SetupWindows.js'

export default {
  methods: {
    cancel() {
      this.$store.dispatch('SetupData/setSetupWindow', SetupWindow.navigation)
    }
  }
}
</script>

<style scoped>
</style>
