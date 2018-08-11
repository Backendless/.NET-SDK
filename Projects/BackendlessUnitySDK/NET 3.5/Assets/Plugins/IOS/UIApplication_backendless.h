#import <Foundation/Foundation.h>

@interface UIApplication (SupressWarnings)
- (void)application:(UIApplication *)application backendlessDidRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken;
- (void)application:(UIApplication *)application backendlessDidFailToRegisterForRemoteNotificationsWithError:(NSError *)err;
- (void)application:(UIApplication *)application backendlessDidReceiveRemoteNotification:(NSDictionary *)userInfo;
- (void)application:(UIApplication *)application backendlessDidReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult result))handler;
- (BOOL)application:(UIApplication *)application backendlessDidFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
@end
