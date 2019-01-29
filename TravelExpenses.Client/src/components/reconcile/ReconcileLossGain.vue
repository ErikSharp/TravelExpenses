<template>
  <div>
    <h3>Loss/Gain</h3>
    <v-btn @click="saveAdjustment">Save</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    saveAdjustment() {
      this.$store.dispatch('showSnackbar', {
        color: 'pink',
        message: 'We have saved your adjustment'
      })
    }
  }
}
</script>

<style scoped></style>
