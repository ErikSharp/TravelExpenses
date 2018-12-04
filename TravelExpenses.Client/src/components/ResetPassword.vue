<template>
  <div>
    <h3>Reset Password</h3>
    <v-btn @click="navToLogin">Cancel</v-btn>
  </div>
</template>

<script>
export default {
  methods: {
    navToLogin() {
      this.$emit('navToLogin')
    }
  }
}
</script>

<style scoped>
</style>
