<template>
  <div>
    <h1>Manage Your Moo-lah</h1>
    <h3>Easily keep track of your expenses while you travel</h3>
    <v-window v-model="authStep">
      <v-window-item>
        <login @register="navToRegister" @resetPassword="navToResetPassword"/>
      </v-window-item>
      <v-window-item>
        <reset-password @navToLogin="navToLogin"/>
      </v-window-item>
      <v-window-item>
        <register @navToLogin="navToLogin"/>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import Login from '@/components/Login.vue'
import Register from '@/components/Register.vue'
import ResetPassword from '@/components/ResetPassword.vue'

export default {
  components: { Login, Register, ResetPassword },
  data() {
    return {
      authStep: 0
    }
  },
  methods: {
    navToLogin() {
      this.authStep = 0
    },
    navToResetPassword() {
      this.authStep = 1
    },
    navToRegister() {
      this.authStep = 2
    }
  }
}
</script>

<style scoped>
</style>
