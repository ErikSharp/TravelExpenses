<template>
  <div>
    <v-icon large dark @click="onClick">keyboard_arrow_left</v-icon>
    <slot />
  </div>
</template>

<script>
import QueryWindows from '@/common/enums/QueryWindows.js'

export default {
  methods: {
    onClick() {
      this.$store.dispatch('Query/setQueryWindow', QueryWindows.queryNav)
    }
  }
}
</script>

<style scoped>
</style>