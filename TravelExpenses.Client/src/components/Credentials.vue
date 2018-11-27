<template>
  <div>
    <v-text-field
      v-if="registering"
      placeholder="Username"
      prepend-inner-icon="person"
    ></v-text-field>
    <v-text-field
      placeholder="Email"
      prepend-inner-icon="email"
    ></v-text-field>
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
    <v-btn dark>{{getButtonText}}</v-btn>
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
