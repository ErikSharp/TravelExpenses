<template>
  <div>
    <h2 class="white--text">Edit category</h2>
    <v-text-field
      v-model.trim="category.categoryName"
      :error-messages="categoryErrors"
      label="Edit name"
      box
      background-color="white"
      color="primary"
      @input="$v.category.$touch()"
      @blur="$v.category.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="edit">Edit</v-btn>
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

const categoryMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.categoryName.toLowerCase())
  return itemsLowered.indexOf(value.categoryName.toLowerCase()) < 0
}

export default {
  props: {
    category: Object
  },
  validations() {
    const result = {
      category: {
        categoryName: {
          required,
          minLength: minLength(3),
          maxLength: maxLength(255)
        },
        categoryMustBeUnique
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
        .dispatch('Category/editCategory', this.category)
        .then(() => this.$emit('cancel'))
    }
  },
  computed: {
    categoryErrors() {
      const errors = []

      if (!this.$v.category.$dirty) return errors

      !this.$v.category.categoryName.maxLength &&
        errors.push(
          `The category can be a maximum of ${
            this.$v.category.categoryName.$params.maxLength.max
          } characters`
        )
      !this.$v.category.categoryName.minLength &&
        errors.push(
          `The category must be a minimum of ${
            this.$v.category.categoryName.$params.minLength.min
          } characters`
        )
      !this.$v.category.categoryMustBeUnique &&
        errors.push('The category must be unique')

      !this.$v.category.categoryName.required &&
        errors.push('A category is required')
      return errors
    },
    items() {
      return this.$store.state.Category.categories
    },
    busy() {
      return this.$store.state.Category.editCategoryBusy
    }
  },
  watch: {
    category() {
      this.$v.$reset()
    }
  }
}
</script>

<style scoped></style>
