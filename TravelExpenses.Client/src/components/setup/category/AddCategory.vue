<template>
  <div>
    <h2 class="white--text">Add additional category</h2>
    <v-text-field
      v-model.trim="categoryName"
      :error-messages="categoryNameErrors"
      label="Enter category name"
      box
      background-color="white"
      color="primary"
      @input="$v.categoryName.$touch()"
      @blur="$v.categoryName.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn
          v-if="editCategory && editCategory.icon"
          :color="editHexColor"
          @click="navColorIcon"
        >
          <v-icon class="white--text">{{ editCategory.icon }}</v-icon>
        </v-btn>
        <v-btn v-else dark color="primary" @click="navColorIcon"
          >Color & Icon</v-btn
        >
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
import CategoryMixin from '@/components/setup/category/CategoryMixin.js'

const categoryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.categoryName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  mixins: [CategoryMixin],
  validations() {
    const base = this.baseValidations()
    base.categoryName['categoryMustBeUnique'] = categoryMustBeUnique

    return base
  },
  methods: {
    add() {
      this.$store.dispatch('Category/addCategory')
    }
  },
  computed: {
    categoryName: {
      get() {
        return this.$store.state.Category.editCategory
          ? this.$store.state.Category.editCategory.categoryName
          : ''
      },
      set(val) {
        this.$store.dispatch('Category/setName', val)
      }
    },
    categoryNameErrors() {
      const errors = []

      if (!this.$v.categoryName.$dirty) return errors

      !this.$v.categoryName.maxLength &&
        errors.push(
          `The category can be a maximum of ${
            this.$v.categoryName.$params.maxLength.max
          } characters`
        )

      !this.$v.categoryName.minLength &&
        errors.push(
          `The category must be a minimum of ${
            this.$v.categoryName.$params.minLength.min
          } characters`
        )

      !this.$v.categoryName.categoryMustBeUnique &&
        errors.push('The category must be unique')

      !this.$v.categoryName.categoryMustNotBeLossGain &&
        errors.push(`The category name must not be ${LossGain}`)

      !this.$v.categoryName.required && errors.push('A category is required')
      return errors
    }
  }
}
</script>

<style scoped>
>>> .v-text-field__details {
  margin-bottom: 0 !important;
}
</style>
