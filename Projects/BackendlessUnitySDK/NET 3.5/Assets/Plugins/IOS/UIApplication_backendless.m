#import <objc/runtime.h>
#import "UIApplication_backendless.h"

char * listenerGameObject = 0;
void setDeviceId()
{
    NSLog(@"[Push]: setDeviceId");
    NSString* uuidString = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
    NSLog(@"[Push]: setDeviceId: %@", uuidString);
    UnitySendMessage(listenerGameObject, "setDeviceId", [uuidString UTF8String]);
}

void setListenerGameObject(char * listenerName)
{
    NSLog(@"[Push]: setListenerGameObject");
    free(listenerGameObject);
    listenerGameObject = 0;
    unsigned long len = strlen(listenerName);
    listenerGameObject = malloc(len+1);
    strcpy(listenerGameObject, listenerName);
    
    setDeviceId();
}

void registerForRemoteNotifications()
{
    NSLog(@"[Push]: registerForRemoteNotifications");
    // ADD BY SIMON TO SUPPORT IOS 8 and earlier (IOS8 stopped supporting registerForRemoteNotifications somehow)
    //-- Set Notification
    if ([[UIApplication sharedApplication]  respondsToSelector:@selector(isRegisteredForRemoteNotifications)])
    {
        // iOS 8 Notifications
        [[UIApplication sharedApplication]  registerUserNotificationSettings:[UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil]];
        
        [[UIApplication sharedApplication]  registerForRemoteNotifications];
    }
    else
    {
        // iOS < 8 Notifications
        [[UIApplication sharedApplication]  registerForRemoteNotificationTypes:
         (UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound)];
    }
    // EOF ADD BY SIMON
}

void unregisterForRemoteNotifications()
{
    NSLog(@"[Push]: unregisterForRemoteNotifications");
    [[UIApplication sharedApplication] unregisterForRemoteNotifications];
    
    UnitySendMessage(listenerGameObject, "unregisterDeviceOnServer", "");
}

@implementation UIApplication (backendless)
+ (void)load
{
    NSLog(@"[Push]: UIApplication");
    method_exchangeImplementations(class_getInstanceMethod(self, @selector(setDelegate:)), class_getInstanceMethod(self, @selector(setBackendlessDelegate:)));
}

BOOL backendlessRunTimeDidFinishLaunching(id self, SEL _cmd, id application, id launchOptions)
{
    NSLog(@"[Push]: backendlessRunTimeDidFinishLaunching");
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
    
    BOOL result = YES;
    if ([self respondsToSelector:@selector(application:backendlessDidFinishLaunchingWithOptions:)]) {
        result = (BOOL) [self application:application backendlessDidFinishLaunchingWithOptions:launchOptions];
    } else {
        [self applicationDidFinishLaunching:application];
        result = YES;
    }
    return result;
}

void backendlessRunTimeDidRegisterForRemoteNotificationsWithDeviceToken(id self, SEL _cmd, id application, id devToken)
{
    NSLog(@"[Push]: backendlessRunTimeDidRegisterForRemoteNotificationsWithDeviceToken");
    if ([self respondsToSelector:@selector(application:backendlessDidRegisterForRemoteNotificationsWithDeviceToken:)]) {
        [self application:application backendlessDidRegisterForRemoteNotificationsWithDeviceToken:devToken];
    }
    const unsigned *tokenBytes = [devToken bytes];
    NSString *hexToken = [NSString stringWithFormat:@"%08x%08x%08x%08x%08x%08x%08x%08x",
                          ntohl(tokenBytes[0]), ntohl(tokenBytes[1]), ntohl(tokenBytes[2]),
                          ntohl(tokenBytes[3]), ntohl(tokenBytes[4]), ntohl(tokenBytes[5]),
                          ntohl(tokenBytes[6]), ntohl(tokenBytes[7])];
    NSLog(@"[Push]: setDeviceToken: %@", hexToken);
    UnitySendMessage(listenerGameObject, "setDeviceToken", [hexToken UTF8String]);
    
    NSString* os = @"IOS";
    NSLog(@"[Push]: setOs: %@", os);
    UnitySendMessage(listenerGameObject, "setOs", [os UTF8String]);
    
    NSString* version = [[UIDevice currentDevice] systemVersion];
    NSLog(@"[Push]: setOsVersion: %@", version);
    UnitySendMessage(listenerGameObject, "setOsVersion", [version UTF8String]);
    
    NSLog(@"[Push]: calling registerDeviceOnServer");
    UnitySendMessage(listenerGameObject, "registerDeviceOnServer", "");
}

void backendlessRunTimeDidFailToRegisterForRemoteNotificationsWithError(id self, SEL _cmd, id application, id error)
{
    NSLog(@"[Push]: backendlessRunTimeDidFailToRegisterForRemoteNotificationsWithError");
    if ([self respondsToSelector:@selector(application:backendlessDidFailToRegisterForRemoteNotificationsWithError:)]) {
        [self application:application backendlessDidFailToRegisterForRemoteNotificationsWithError:error];
    }
    NSString *errorString = [error description];
    const char * str = [errorString UTF8String];
    UnitySendMessage(listenerGameObject, "onDidFailToRegisterForRemoteNotificationsWithError", str);
}

void backendlessRunTimeDidReceiveRemoteNotification(id self, SEL _cmd, id application, id userInfo)
{
    NSLog(@"[Push]: backendlessRunTimeDidReceiveRemoteNotification");
    if ([self respondsToSelector:@selector(application:backendlessDidReceiveRemoteNotification:)]) {
        [self application:application backendlessDidReceiveRemoteNotification:userInfo];
    }
    
    // ADD BY SIMON
    if ([userInfo objectForKey:@"url"] != NULL && ![[userInfo objectForKey:@"url"] isEqualToString:@""]) {
        NSLog(@"Opening URL in Push Notification: %@", [userInfo objectForKey:@"url"]);
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:[userInfo objectForKey:@"url"]]];
    }
    // EOF ADD BY SIMON
    
    UnitySendMessage(listenerGameObject, "onPushMessage", [[[userInfo objectForKey:@"aps"] objectForKey:@"alert"] UTF8String]);
}

void backendlessRunTimeApplicationWillEnterForeground(id self, SEL _cmd, id application)
{
    NSLog(@"[Push]: backendlessRunTimeApplicationWillEnterForeground");
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
}

static void exchangeMethodImplementations(Class class, SEL oldMethod, SEL newMethod, IMP impl, const char * signature)
{
    NSLog(@"[Push]: exchangeMethodImplementations");
    Method method = nil;
    method = class_getInstanceMethod(class, oldMethod);
    if (method) {
        class_addMethod(class, newMethod, impl, signature);
        method_exchangeImplementations(class_getInstanceMethod(class, oldMethod), class_getInstanceMethod(class, newMethod));
    } else {
        class_addMethod(class, oldMethod, impl, signature);
    }
}

- (void) setBackendlessDelegate:(id<UIApplicationDelegate>)delegate
{
    NSLog(@"[Push]: setBackendlessDelegate");
    static Class delegateClass = nil;
    
    if(delegateClass == [delegate class]) {
        [self setBackendlessDelegate:delegate];
        return;
    }
    
    delegateClass = [delegate class];
    
    exchangeMethodImplementations(delegateClass, @selector(application:didFinishLaunchingWithOptions:),
                                  @selector(application:backendlessDidFinishLaunchingWithOptions:), (IMP)backendlessRunTimeDidFinishLaunching, "v@:::");
    
    
    exchangeMethodImplementations(delegateClass, @selector(application:didRegisterForRemoteNotificationsWithDeviceToken:),
                                  @selector(application:backendlessDidRegisterForRemoteNotificationsWithDeviceToken:), (IMP)backendlessRunTimeDidRegisterForRemoteNotificationsWithDeviceToken, "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didFailToRegisterForRemoteNotificationsWithError:),
                                  @selector(application:backendlessDidFailToRegisterForRemoteNotificationsWithError:), (IMP)backendlessRunTimeDidFailToRegisterForRemoteNotificationsWithError, "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didReceiveRemoteNotification:),
                                  @selector(application:backendlessDidReceiveRemoteNotification:), (IMP)backendlessRunTimeDidReceiveRemoteNotification, "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(applicationWillEnterForeground:),
                                  @selector(backendlessApplicationWillEnterForeground:), (IMP)backendlessRunTimeApplicationWillEnterForeground, "v@::");
    
    [self setBackendlessDelegate:delegate];
}

@end
