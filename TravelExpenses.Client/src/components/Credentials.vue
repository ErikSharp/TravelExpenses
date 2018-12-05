<template>
  <div>
    <v-text-field
      v-model.trim="username"
      :error-messages="usernameErrors"
      v-if="registering"
      placeholder="Username"
      prepend-inner-icon="person"
      @input="$v.username.$touch()"
      @blur="$v.username.$touch()"
    ></v-text-field>
    <v-text-field
      v-model.trim="email"
      :error-messages="emailErrors"
      placeholder="Email"
      prepend-inner-icon="email"
      @input="$v.email.$touch()"
      @blur="$v.email.$touch()"
    ></v-text-field>
    <v-text-field
      v-model.trim="password"
      :error-messages="passwordErrors"
      placeholder="Password"
      prepend-inner-icon="vpn_key"
      :append-icon="visibilityIcon"
      :type="passwordInputType"
      @click:append="toggleVisibility"
      @input="$v.password.$touch()"
      @blur="$v.password.$touch()"
    ></v-text-field>
    <v-text-field
      v-if="registering"
      v-model.trim="passwordConfirmation"
      :error-messages="passwordConfirmationErrors"
      placeholder="Confirm Password"
      prepend-inner-icon="vpn_key"
      :append-icon="getConfirmIcon"
      :type="passwordInputType"
      @input="$v.passwordConfirmation.$touch()"
      @blur="$v.passwordConfirmation.$touch()"
    ></v-text-field>
    <v-btn :disabled="$v.$invalid" @click="submit">{{getButtonText}}</v-btn>
  </div>
</template>

<script>
import {
  required,
  requiredIf,
  minLength,
  maxLength,
  sameAs,
  email
} from 'vuelidate/lib/validators'

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
  validations() {
    const result = {
      email: {
        required,
        email
      },
      password: {
        required,
        minLength: minLength(6),
        maxLength: maxLength(50)
      }
    }

    if (this.registering) {
      result['username'] = {
        requiredIf: requiredIf(c => c.registering),
        minLength: minLength(3),
        maxLength: maxLength(60)
      }

      result['passwordConfirmation'] = {
        requiredIf: requiredIf(c => c.registering),
        sameAsPassword: sameAs('password')
      }
    }

    return result
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
      return !this.$v.passwordConfirmation.$invalid ? 'done' : ''
    },
    getButtonText() {
      return this.registering ? 'REGISTER' : 'LOG IN'
    },
    usernameErrors() {
      const errors = []
      if (!this.$v.username.$dirty) return errors

      !this.$v.username.maxLength &&
        errors.push(
          `The username can be a maximum of ${
            this.$v.username.$params.maxLength.max
          } characters`
        )
      !this.$v.username.minLength &&
        errors.push(
          `The username must be a minimum of ${
            this.$v.username.$params.minLength.min
          } characters`
        )
      !this.$v.username.requiredIf && errors.push('A username is required')
      return errors
    },
    emailErrors() {
      const errors = []
      if (!this.$v.email.$dirty) return errors

      !this.$v.email.email &&
        errors.push('You must enter a valid email address')
      !this.$v.email.required && errors.push('An email address is required')
      return errors
    },
    passwordErrors() {
      const errors = []
      if (!this.$v.password.$dirty) return errors

      !this.$v.password.maxLength &&
        errors.push(
          `The password can be a maximum of ${
            this.$v.password.$params.maxLength.max
          } characters`
        )
      !this.$v.password.minLength &&
        errors.push(
          `The password must be a minimum of ${
            this.$v.password.$params.minLength.min
          } characters`
        )
      !this.$v.password.required && errors.push('A password is required')
      return errors
    },
    passwordConfirmationErrors() {
      const errors = []

      if (!this.$v.passwordConfirmation.$dirty) return errors

      !this.$v.passwordConfirmation.requiredIf &&
        errors.push('A password confirmation is required')
      !this.$v.passwordConfirmation.sameAsPassword &&
        errors.push('The passwords must match')
      return errors
    }
  }
}
</script>

<style scoped>
</style>
