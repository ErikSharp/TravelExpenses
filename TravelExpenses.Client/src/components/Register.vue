<template>
  <div>
    <h3>Register</h3>
    <credentials registering/>
    <p>
      Already have an account?
      <span class="click-text" @click="login">Log in here</span>
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
    login() {
      this.$emit('navToLogin')
    }
  }
}
</script>

<style scoped>
</style>
