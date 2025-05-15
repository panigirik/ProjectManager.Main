module.exports = {
    root: true,
    env: {
      node: true
    },
    extends: [
      'plugin:vue/vue3-essential',
      'eslint:recommended'
    ],
    parserOptions: {
      ecmaVersion: 2020,
      sourceType: 'module'
    },
    globals: {
      defineProps: 'readonly',
      defineEmits: 'readonly',
      defineExpose: 'readonly',
      withDefaults: 'readonly'
    },
    rules: {
      'vue/multi-word-component-names': 'off'
    }
  }
  