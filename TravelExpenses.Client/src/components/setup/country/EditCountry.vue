<template>
  <div>
    <h2 class="white--text">Edit country</h2>
    <v-text-field
      v-model.trim="country.countryName"
      :error-messages="countryErrors"
      label="Edit name"
      box
      background-color="white"
      color="primary"
      @input="$v.country.$touch()"
      @blur="$v.country.$touch()"
    ></v-text-field>
    <v-container>
      <v-layout row wrap>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="edit">Edit</v-btn>
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
  return itemsLowered.indexOf(value.countryName.toLowerCase()) < 0
}

export default {
  props: {
    country: Object
  },
  validations() {
    const result = {
      country: {
        countryName: {
          required,
          minLength: minLength(3),
          maxLength: maxLength(255)
        },
        countryMustBeUnique
      }
    }

    return result
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    edit() {
      this.$store
        .dispatch('Country/editCountry', this.country)
        .then(() => this.$emit('cancel'))
    }
  },
  computed: {
    countryErrors() {
      const errors = []

      if (!this.$v.country.$dirty) return errors

      !this.$v.country.countryName.maxLength &&
        errors.push(
          `The country can be a maximum of ${
            this.$v.country.countryName.$params.maxLength.max
          } characters`
        )
      !this.$v.country.countryName.minLength &&
        errors.push(
          `The country must be a minimum of ${
            this.$v.country.countryName.$params.minLength.min
          } characters`
        )
      !this.$v.country.countryMustBeUnique &&
        errors.push('The country must be unique')

      !this.$v.country.countryName.required &&
        errors.push('A country is required')
      return errors
    },
    items() {
      return this.$store.state.Country.countries
    },
    busy() {
      return this.$store.state.Country.editCountryBusy
    }
  },
  watch: {
    country() {
      this.$v.$reset()
    }
  }
}
</script>

<style scoped>
</style>
