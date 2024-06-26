<template>
  <div class="signup">
    <el-card class="signup-card">
      <h2 class="signup-title">Sign Up</h2>
      <!-- Sign Up Form -->
      <el-form
        ref="signupForm"
        :model="signupForm"
        :rules="signupRules"
        label-width="0"
        v-loading="loading"
      >
        <!-- Username Field -->
        <el-form-item prop="username" class="form-item">
          <el-input
            v-model="signupForm.username"
            placeholder="User"
            class="input-field"
            prefix-icon="user"
          ></el-input>
        </el-form-item>
        <!-- Email Field -->
        <el-form-item prop="email" class="form-item">
          <el-input
            v-model="signupForm.email"
            placeholder="Email"
            class="input-field"
            prefix-icon="message"
          ></el-input>
        </el-form-item>
        <!-- Password Field -->
        <el-form-item prop="password" class="form-item">
          <el-input
            v-model="signupForm.password"
            placeholder="Password"
            show-password
            class="input-field"
            prefix-icon="lock"
          ></el-input>
        </el-form-item>
        <!-- Confirm Password Field -->
        <el-form-item prop="confirmPassword" class="form-item">
          <el-input
            v-model="signupForm.confirmPassword"
            placeholder="Confirm Password"
            show-password
            class="input-field"
            prefix-icon="lock"
          ></el-input>
        </el-form-item>
        <!-- Sign Up Button -->
        <el-form-item>
          <el-button type="primary" @click="register" class="signup-button">
            Sign Up
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <!-- Display Error Message -->
    <div v-if="error" class="error">{{ error }}</div>
  </div>
</template>

<script>
import axios from "axios";
import {
  signupEndPoint,
  sendEmailEndPoint,
  loginEndPoint,
} from "~/constants/endpoints";
import CryptoJS from "crypto-js";
import { signUpEmailTemplate } from "~/constants/emails";
import { useSessionStore } from "~/stores/session";

export default {
  setup() {
    const session = useSessionStore();
    return { session };
  },
  data() {
    return {
      // Sign Up Form Data and Rules
      signupForm: {
        username: "",
        email: "",
        password: "",
        confirmPassword: "",
      },
      signupRules: {
        username: [
          {
            required: true,
            message: "Please, introduce a username",
            trigger: "blur",
          },
        ],
        email: [
          {
            required: true,
            message: "Please, introduce an email",
            trigger: "blur",
          },
          {
            type: "email",
            message: "Please, introduce a valid email",
            trigger: "blur,change",
          },
        ],
        password: [
          {
            required: true,
            message: "Please, introduce a password",
            trigger: "blur",
          },
          {
            min: 6,
            message: "Must contain at least 6 characters",
            trigger: "blur",
          },
          {
            validator: this.validatePassword,
            trigger: "blur",
          },
        ],
        confirmPassword: [
          {
            required: true,
            message: "Please, repeat your password",
            trigger: "blur",
          },
          {
            validator: this.validateConfirmPassword,
            trigger: "blur",
          },
        ],
      },
      error: "",
      loading: false,
    };
  },
  methods: {
    // Encrypts the provided text
    encrypt(text) {
      return CryptoJS.enc.Base64.stringify(CryptoJS.enc.Utf8.parse(text));
    },
    // Handles user registration
    async register() {
      this.$refs.signupForm.validate(async (valid) => {
        if (valid) {
          this.loading = true;
          try {
            // Send registration request
            let response = await axios.post(
              `${signupEndPoint}?username=${this.signupForm.username.toLowerCase()}&email=${
                this.signupForm.email
              }&password=${this.encrypt(this.signupForm.password)}`
            );
            // Login after successful registration
            response = await axios.post(`${loginEndPoint}`, {
              email: this.signupForm.username.toLowerCase(),
              password: this.encrypt(this.signupForm.password),
            });

            const token = response.data.token;
            console.log(token);
            // Updating session and redirecting on successful login
            const sessionStore = useSessionStore();
            sessionStore.login(this.signupForm.username, token);

            this.loading = false;
            // Redirect to dashboard
            this.$router.push("/dashboard");
          } catch (error) {
            this.loading = false;
            console.error("Failed signup", error);
            if (error.response.status === 409) {
              this.$message.error("Username or email already in use.");
            } else {
              this.$message.error("An error ocurred. Try again later...");
            }
          }

          try {
            // Send confirmation email
            await axios.post(
              `${sendEmailEndPoint}?mailFrom=${this.signupForm.email}&subject=${signUpEmailTemplate.subject}&body=${signUpEmailTemplate.body}`
            );
          } catch (error) {}
        } else {
          console.log("Not valid form");
        }
      });
    },
    // Validates if passwords match
    validateConfirmPassword(rule, value, callback) {
      if (value !== this.signupForm.password) {
        callback(new Error("Passwords don´t match"));
      } else {
        callback();
      }
    },
    // Validates password complexity
    validatePassword(rule, value, callback) {
      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasSpecialChar = /[\W_]/.test(value);

      if (!hasUpperCase) {
        callback(new Error("Must contain at least one uppercase"));
      } else if (!hasLowerCase) {
        callback(new Error("Must contain at least one lowercase"));
      } else if (!hasSpecialChar) {
        callback(new Error("Must contain special characters"));
      } else {
        callback();
      }
    },
  },
};
</script>

<style lang="scss" scoped>
@import "@/assets/styles/variables.scss";

.signup {
  display: flex;
  justify-content: center;
  align-items: center;
}

.signup-card {
  width: 300px;
  padding: $spacing-medium;
}

.signup-title {
  text-align: center;
  font-size: $font-size-large;
  margin-bottom: $spacing-medium;
}

.form-item {
  margin-bottom: $spacing-small;
}

.input-field {
  border-radius: 0;
  font-size: $font-size-small;
  margin-top: $spacing-medium;
}

.signup-button {
  width: 100%;
  border-radius: 10;
  background-color: $color-primary;
  transition: background-color 0.3s ease;
  margin-top: $spacing-medium;
  color: black;
}

.signup-button:hover {
  background-color: darken($color-primary, 10%);
}

.error {
  color: red;
  text-align: center;
  margin-top: $spacing-small;
}
</style>
