<template>
  <div>
    <h2 class="white--text">Add additional category</h2>
    <v-text-field
      v-model.trim="category"
      :error-messages="categoryErrors"
      label="Enter category name"
      box
      background-color="white"
      color="primary"
      @input="$v.category.$touch()"
      @blur="$v.category.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn
          dark
          color="primary"
          :disabled="$v.$invalid"
          :loading="busy"
          @click="add"
          >Add</v-btn
        >
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'
import { LossGain } from '@/common/constants/StringConstants.js'

const categoryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.categoryName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

const categoryMustNotBeLossGain = value => {
  return value.toLowerCase() !== LossGain.toLowerCase()
}

export default {
  validations() {
    const result = {
      category: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        categoryMustBeUnique,
        categoryMustNotBeLossGain
      }
    }

    return result
  },
  data() {
    return {
      category: ''
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    add() {
      this.$store.dispatch('Category/addCategories', [this.category])
    }
  },
  computed: {
    categoryErrors() {
      const errors = []

      if (!this.$v.category.$dirty) return errors

      !this.$v.category.maxLength &&
        errors.push(
          `The category can be a maximum of ${
            this.$v.category.$params.maxLength.max
          } characters`
        )
      !this.$v.category.minLength &&
        errors.push(
          `The category must be a minimum of ${
            this.$v.category.$params.minLength.min
          } characters`
        )
      !this.$v.category.categoryMustBeUnique &&
        errors.push('The category must be unique')
      !this.$v.category.categoryMustNotBeLossGain &&
        errors.push(`The category name must not be ${LossGain}`)

      !this.$v.category.required && errors.push('A category is required')
      return errors
    },
    items() {
      return this.$store.state.Category.categories
    },
    busy() {
      return this.$store.state.Category.addCategoryBusy
    }
  },
  watch: {
    items() {
      this.category = ''
      this.$v.$reset()
    }
  }
}
</script>

<style scoped></style>
