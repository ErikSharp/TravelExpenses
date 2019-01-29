<template>
  <div>
    <h3>New Withdrawal</h3>
    <v-btn @click="done">Done</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    done() {
      this.$emit('done')
    }
  }
}
</script>

<style scoped></style>
