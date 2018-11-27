<template>
  <div>
    <credentials/>
    <p>Forgot your password? <span class="click-text" @click="resetPassword">Reset</span></p>
    <p>Don't have an account? <span class="click-text" @click="register">Register</span></p>
  </div>
</template>

<script>
import Credentials from '@/components/Credentials.vue'
import * as AuthSteps from '@/common/AuthSteps.js'

export default {
  components: {
    Credentials
  },
  methods: {
    register() {
      this.$store.dispatch('setAuthStep', AuthSteps.registerStep)
    },
    resetPassword() {
      this.$store.dispatch('setAuthStep', AuthSteps.resetPasswordStep)
    }
  }
}
</script>

<style scoped>
</style>
