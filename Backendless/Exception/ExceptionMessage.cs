using System;

namespace BackendlessAPI.Exception
{
  public class ExceptionMessage
  {
    public const string NULL_OR_EMPTY_TEMPLATE = "{0} cannot be null or empty";
    public const string NULL_TEMPLATE = "{0} cannot be null";
    public const string NULL_EMAIL_ENVELOPE = "EmailEnvelope can not be null.";
    public const string NUL_WEBBROWSER = "WebBrowser cannot be null";
    public const string ILLEGAL_ARGUMENT_EXCEPTION = "IllegalArgumentException";
    public const string SERVER_ERROR = "Server returned an error.";
    public const string CLIENT_ERROR = "Internal client exception.";
    public const string NULL_EMAIL = "Email cannot be null or empty";

    public const string WRONG_MANIFEST = "Wrong dependencies at the manifest";
    public const string NOT_INITIALIZED = "Backendless application was not initialized";

    public const string NULL_USER = "User cannot be null or empty.";
    public const string NULL_ROLE_NAME = "Role cannot be null or empty";
    public const string NULL_PASSWORD = "User password cannot be null or empty.";
    public const string NULL_LOGIN = "User login cannot be null or empty.";

    public const string NULL_CONTEXT =
        "Context cannot be null. Use Backendless.initApp( String applicationId, String secretKey ) for proper initialization.";

    public const String NULL_CATEGORY_NAME = "Category name cannot be null or empty.";
    public const String NULL_GEOPOINT = "Geopoint cannot be null.";
    public const String DEFAULT_CATEGORY_NAME = "cannot add or delete a default category name.";

    public const String NULL_BULKCREATE = "Collection of objects in bulk create cannot be null";
    public const String NULL_WHERE = "Where clause cannot be null or empty";
    public const String NULL_ENTITY = "Entity cannot be null.";
    public const String NULL_ENTITY_NAME = "Entity name cannot be null or empty.";
    public const String NULL_ID = "Object id cannot be null or empty.";

    public const String NULL_UNIT = "Unit type cannot be null or empty.";

    public const String NULL_CHANNEL_NAME = "Channel name cannot be null or empty.";
    public const String NULL_MESSAGE = "Message cannot be null. Use an empty String instead.";
    public const String NULL_MESSAGE_ID = "Message id cannot be null or empty.";
    public const String NULL_SUBSCRIPTION_ID = "Subscription id cannot be null or empty.";

    public const String NULL_FILE = "File reference cannot be null.";
    public const String NULL_PATH = "File path cannot be null or empty.";
    public const String NULL_NAME = "File name cannot be null or empty.";
    public const String NULL_FILE_CONTENTS = "File contents cannot be null or empty";
    public const String NULL_BITMAP = "Bitmap cannot be null";
    public const String NULL_COMPRESS_FORMAT = "CompressFormat cannot be null";

    public const String NULL_IDENTITY = "Identity cannot be null";

    public const String NULL_APPLICATION_ID = "Application id cannot be null";
    public const String NULL_SECRET_KEY = "API key cannot be null";
    public const String NULL_DEVICE_TOKEN = "Null device token received";

    public const String WRONG_RADIUS = "Wrong radius value.";

    public const String WRONG_SEARCH_RECTANGLE_QUERY =
        "Wrong rectangle search query. It should contain four points.";

    public const String WRONG_FILE = "cannot read the file.";

    public const String WRONG_LATITUDE_VALUE = "Latitude value should be between -90 and 90.";
    public const String WRONG_LONGITUDE_VALUE = "Longitude value should be between -180 and 180.";

    public const String WRONG_USER_ID = "User not logged in or wrong user id.";

    public const String WRONG_GEO_QUERY = "Could not understand Geo query options. Specify any.";

    public const String INCONSISTENT_GEO_QUERY =
        "Inconsistent geo query. Query should not contain both rectangle and radius search parameters.";

    public const String WRONG_OFFSET = "Offset cannot have a negative value.";
    public const String WRONG_PAGE_SIZE = "Pagesize cannot have a negative value.";

    public const String WRONG_SUBSCRIPTION_STATE = "cannot resume a subscription, which is not paused.";
    public const String WRONG_EXPIRATION_DATE = "Wrong expiration date";

    public const String WRONG_POLLING_INTERVAL = "Wrong polling interval";
    public const String DEVICE_NOT_REGISTERED = "Device is not registered.";

    public const String NOT_READABLE_FILE = "File is not readable.";
    public const String FILE_UPLOAD_ERROR = "Could not upload a file";

    public const String ENTITY_MISSING_DEFAULT_CONSTRUCTOR = "No default noargument constructor";
    public const String ENTITY_WRONG_OBJECT_ID_FIELD_TYPE = "Wrong type of the objectId field";
    public const String ENTITY_WRONG_CREATED_FIELD_TYPE = "Wrong type of the created field";
    public const String ENTITY_WRONG_UPDATED_FIELD_TYPE = "Wrong type of the updated field";
    public const String WRONG_ENTITY_TYPE = "Wrong entity type";

    public const String LOCAL_FILE_EXISTS = "Local file exists";
    public const String WRONG_REMOTE_PATH = "Remote path cannot be empty";

    public const String NO_DEVICEID_CAPABILITY =
        "In order to use Backendless SDK in WindowsPhone environment, application should have an 'ID_CAP_IDENTITY_DEVICE' capability";
    public const String NULL_GEO_QUERY = "Geo query should not be null";
    public const String INCONSISTENT_GEO_RELATIVE = "Geo query should contain relative metadata and a threshold for a relative search";
    public const String NULL_BODYPARTS = "BodyParts cannot be null";
    public const String NULL_SUBJECT = "Subject cannot be null";
    public const String NULL_RECIPIENTS = "Recipients cannot be empty";
    public const String NULL_ATTACHMENTS = "Attachments cannot be null";
    public const String NULL_EMPTY_TEMPLATE_NAME = "Email template name cannot be null or empty";

    public const String GEOFENCE_ALREADY_MONITORING = "The {0} geofence is already being monitored. Monitoring of the geofence must be stopped before you start it again";
    public const String GEOFENCES_MONITORING = "Cannot start geofence monitoring for all available geofences. There is another monitoring session in progress on the client-side. Make sure to stop all monitoring sessions before starting it for all available geo fences.";

	  public const String CHANNEL_NAME_TOO_LONG = "Channel names cannot be longer than 46 characters";

    public const String NULL_MAP = "Entity dictionary cannot be null";
    public const String NULL_OBJECT_ID_IN_OBJECT_MAP = "Object dictionary must contain String objectId and objectId cannot be null";
    public const String NULL_OBJECT_ID_IN_INSTANCE = "Instance must contain objectId and objectId can not be null";
    public const String NULL_BULK = "Array of objects/dictionaries cannot be null";
    public const String REF_TYPE_NOT_SUPPORT = "This operation result is not supported in this operation";
    public const String OP_RESULT_ID_ALREADY_PRESENT = "This opResultId is already present. OpResultId must be unique";
    public const String OP_RESULT_FROM_THIS_OPERATION_NOT_SUPPORT_IN_THIS_PLACE = "OpResult/OpResultValueReference from this operation in this place is not supported";
    public const String NULL_INSTANCE = "Entity class cannot be null";
    public const String LIST_OPERATIONS_NULL = "List of operations in unitOfWork can not be null or empty";
    public const String LIST_NOT_INSTANCES = "Array can be only of instances";
    public const String NULL_OP_RESULT = "OpResult cannot be null";
    public const String NULL_OP_RESULT_VALUE_REFERENCE = "OpResultValueReference cannot be null";
    public const String OP_RESULT_INDEX_YES_PROP_NAME_NOT = "This operation result in this operation must resolve only to resultIndex";
    public const String NULL_PARENT_TABLE_NAME = "Parent table name can not be null or empty";
    public const String NULL_RELATION_COLUMN_NAME = "Relation column name can not be null or empty";
    public const String RELATION_USE_LIST_OF_MAPS = "Unable to execute the relation operation. Use the relation method which accepts a list of Map child objects";
  }
}