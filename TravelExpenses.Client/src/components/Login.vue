<template>
  <div>
    <credentials />
    <p>
      Forgot your password?
      <span class="click-text" @click="resetPassword">Reset</span>
    </p>
    <p>
      Don't have an account?
      <span class="click-text" @click="register">Register</span>
    </p>
  </div>
</template>

<script>
import Credentials from '@/components/Credentials.vue'

export default {
  components: {
    Credentials
  },
  methods: {
    register() {
      this.$emit('register')
    },
    resetPassword() {
      this.$emit('resetPassword')
    }
  }
}
</script>

<style scoped></style>
