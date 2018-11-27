<template>
  <div>
    <h1>Manage Your Moo-lah</h1>
    <h3>Easily keep track of your expenses while you travel</h3>
    <v-window v-model="authStep">
      <v-window-item>
        Reset the password here
      </v-window-item>
      <v-window-item>
        <login/>
      </v-window-item>
      <v-window-item>
        <register/>
      </v-window-item>
    </v-window>
  </div>
</template>

<script>
import Login from '@/components/Login.vue'
import Register from '@/components/Register.vue'

export default {
  components: { Login, Register },
  data() {
    return {}
  },
  computed: {
    authStep: {
      get() {
        return this.$store.state.authStep
      },
      set(newValue) {
        return this.$store.dispatch('setAuthStep', newValue)
      }
    }
  }
}
</script>

<style scoped>
</style>
