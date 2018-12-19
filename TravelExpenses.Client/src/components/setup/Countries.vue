<template>
  <div>
    <h2 class="white--text">Added countries</h2>
    <v-list subheader>
      <template v-for="(item, index) in listItems">
        <v-list-tile @click="edit(item)" :key="item">
          <v-list-tile-content>
            <v-list-tile-title v-text="item"></v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider v-if="index + 1 < items.length" class="my-0" :key="item + 'div'"></v-divider>
      </template>
    </v-list>
    <hr class="my-5">
    <h2 class="white--text">Add additional countries</h2>
    <v-text-field
      v-model.trim="newCountry"
      :error-messages="newCountryErrors"
      label="Enter country name"
      box
      background-color="white"
      color="primary"
      @input="$v.newCountry.$touch()"
      @blur="$v.newCountry.$touch()"
    ></v-text-field>
    <v-container>
      <v-layout row wrap>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" :disabled="$v.$invalid" @click="add">Add</v-btn>
        </v-flex>
        <v-flex xs4 offset-xs2>
          <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import SetupWindow from '@/common/enums/SetupWindows.js'
import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import sortBy from 'lodash/sortBy'

const countryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  data() {
    return {
      items: ['Germany', 'France', 'Austria', 'Croatia'],
      newCountry: ''
    }
  },
  validations() {
    const result = {
      newCountry: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        countryMustBeUnique
      }
    }

    return result
  },
  methods: {
    cancel() {
      this.$store.dispatch('SetupData/setSetupWindow', SetupWindow.navigation)
    },
    add() {
      alert('add')
    },
    edit(item) {
      alert(item)
    }
  },
  computed: {
    listItems() {
      return sortBy(this.items)
    },
    newCountryErrors() {
      const errors = []

      if (!this.$v.newCountry.$dirty) return errors

      !this.$v.newCountry.maxLength &&
        errors.push(
          `The country can be a maximum of ${
            this.$v.newCountry.$params.maxLength.max
          } characters`
        )
      !this.$v.newCountry.minLength &&
        errors.push(
          `The country must be a minimum of ${
            this.$v.newCountry.$params.minLength.min
          } characters`
        )
      !this.$v.newCountry.countryMustBeUnique &&
        errors.push('The country must be unique')

      !this.$v.newCountry.required && errors.push('A country is required')
      return errors
    }
  }
}
</script>

<style scoped>
</style>
