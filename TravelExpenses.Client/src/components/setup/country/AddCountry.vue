<template>
  <div>
    <h2 class="white--text">Add additional country</h2>
    <v-text-field
      v-model.trim="country"
      :error-messages="countryErrors"
      label="Enter country name"
      box
      background-color="white"
      color="primary"
      @input="$v.country.$touch()"
      @blur="$v.country.$touch()"
    ></v-text-field>
    <v-container>
      <v-layout row wrap>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="add">Add</v-btn>
        </v-flex>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

const countryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.countryName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  validations() {
    const result = {
      country: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        countryMustBeUnique
      }
    }

    return result
  },
  data() {
    return {
      country: ''
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    add() {
      this.$store.dispatch('Country/addCountry', this.country)
    }
  },
  computed: {
    countryErrors() {
      const errors = []

      if (!this.$v.country.$dirty) return errors

      !this.$v.country.maxLength &&
        errors.push(
          `The country can be a maximum of ${
            this.$v.country.$params.maxLength.max
          } characters`
        )
      !this.$v.country.minLength &&
        errors.push(
          `The country must be a minimum of ${
            this.$v.country.$params.minLength.min
          } characters`
        )
      !this.$v.country.countryMustBeUnique &&
        errors.push('The country must be unique')

      !this.$v.country.required && errors.push('A country is required')
      return errors
    },
    items() {
      return this.$store.state.Country.countries
    },
    busy() {
      return this.$store.state.Country.addCountryBusy
    }
  },
  watch: {
    items() {
      this.country = ''
      this.$v.$reset()
    }
  }
}
</script>

<style scoped>
</style>
