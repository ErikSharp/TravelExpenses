<template>
  <div>
    <h1>Lets add a new one</h1>
    <v-btn @click="cancel">Cancel</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    cancel() {
      this.$emit('done')
    }
  }
}
</script>

<style scoped>
</style>
