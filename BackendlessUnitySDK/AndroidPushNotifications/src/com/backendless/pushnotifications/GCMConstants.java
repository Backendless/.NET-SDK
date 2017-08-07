/*
Copyright 2015 Backendless Corp. All Rights Reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

package com.backendless.pushnotifications;

public class GCMConstants
{
  public static final String INTENT_TO_GCM_REGISTRATION = "com.google.android.c2dm.intent.REGISTER";
  public static final String INTENT_TO_GCM_UNREGISTRATION = "com.google.android.c2dm.intent.UNREGISTER";
  public static final String INTENT_FROM_GCM_REGISTRATION_CALLBACK = "com.google.android.c2dm.intent.REGISTRATION";
  public static final String INTENT_FROM_GCM_LIBRARY_RETRY = "com.google.android.gcm.intent.RETRY";
  public static final String INTENT_FROM_GCM_MESSAGE = "com.google.android.c2dm.intent.RECEIVE";
  public static final String EXTRA_SENDER = "sender";
  public static final String EXTRA_APPLICATION_PENDING_INTENT = "app";
  public static final String EXTRA_UNREGISTERED = "unregistered";
  public static final String EXTRA_ERROR = "error";
  public static final String EXTRA_IS_INTERNAL = "internal_registration";
  public static final String EXTRA_REGISTRATION_ID = "registration_id";
  public static final String EXTRA_DEVICE_TOKEN = "device_token";
  public static final String EXTRA_SPECIAL_MESSAGE = "message_type";
  public static final String VALUE_DELETED_MESSAGES = "deleted_messages";
  public static final String EXTRA_TOTAL_DELETED = "total_deleted";

  public static final String ERROR_SERVICE_NOT_AVAILABLE = "SERVICE_NOT_AVAILABLE";
  public static final String ERROR_ACCOUNT_MISSING = "ACCOUNT_MISSING";
  public static final String ERROR_AUTHENTICATION_FAILED = "AUTHENTICATION_FAILED";
  public static final String ERROR_INVALID_PARAMETERS = "INVALID_PARAMETERS";
  public static final String ERROR_INVALID_SENDER = "INVALID_SENDER";
  public static final String ERROR_PHONE_REGISTRATION_ERROR = "PHONE_REGISTRATION_ERROR";

  public static final String PERMISSION_ANDROID_INTERNET =  "android.permission.INTERNET";
  public static final String PERMISSION_ANDROID_ACCOUNTS = "android.permission.GET_ACCOUNTS";
  public static final String PERMISSION_GCM_MESSAGE = "com.google.android.c2dm.permission.RECEIVE";
  public static final String PERMISSION_GCM_INTENTS = "com.google.android.c2dm.permission.SEND";

  private GCMConstants()
  {
    throw new UnsupportedOperationException();
  }
}