<template>
  <div>
    <v-text-field
      v-model="username"
      v-if="registering"
      placeholder="Username"
      prepend-inner-icon="person"
    ></v-text-field>
    <v-text-field v-model="email" placeholder="Email" prepend-inner-icon="email"></v-text-field>
    <v-text-field
      v-model="password"
      placeholder="Password"
      prepend-inner-icon="vpn_key"
      :append-icon="visibilityIcon"
      :type="passwordInputType"
      @click:append="toggleVisibility"
    ></v-text-field>
    <v-text-field
      v-if="registering"
      v-model="passwordConfirmation"
      placeholder="Confirm Password"
      prepend-inner-icon="vpn_key"
      :append-icon="getConfirmIcon"
      :type="passwordInputType"
    ></v-text-field>
    <v-btn dark @click="submit">{{getButtonText}}</v-btn>
  </div>
</template>

<script>
export default {
  props: {
    registering: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      passwordInputType: 'password',
      visibilityIcon: 'visibility',
      username: '',
      email: '',
      password: '',
      passwordConfirmation: ''
    }
  },
  methods: {
    toggleVisibility() {
      if (this.passwordInputType === 'password') {
        this.passwordInputType = 'text'
        this.visibilityIcon = 'visibility_off'
      } else {
        this.passwordInputType = 'password'
        this.visibilityIcon = 'visibility'
      }
    },
    submit() {
      if (this.registering) {
        this.$store.dispatch('Authentication/registerUser', {
          username: this.username,
          email: this.email,
          password: this.password
        })
      } else {
        this.$store.dispatch('Authentication/login', {
          email: this.email,
          password: this.password
        })
      }
    }
  },
  computed: {
    getConfirmIcon() {
      let match = this.password === this.passwordConfirmation ? 'done' : ''
      return this.password && match
    },
    getButtonText() {
      return this.registering ? 'REGISTER' : 'LOG IN'
    }
  }
}
</script>

<style scoped>
</style>
